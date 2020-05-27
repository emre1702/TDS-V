import { Component, OnInit, OnDestroy, AfterViewChecked, ChangeDetectorRef, ViewChild, HostListener, ElementRef, ChangeDetectionStrategy, NgZone } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from '../../../enums/dtoclientevent.enum';
import { MatInput } from '@angular/material';
import { SettingsService } from '../../../services/settings.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';
import { MentionConfig } from '../../../extensions/mention/mentionConfig';
import { MentionDirective } from '../../../extensions/mention/mentionDirective';
import { animate, style, AnimationBuilder, AnimationPlayer } from '@angular/animations';
import { DFromServerEvent } from '../../../enums/dfromserverevent.enum';

declare const mp: {
    events: {
        add(eventName: string, method: () => void): void;
        remove(eventName: string, method: () => void): void;
    }

    trigger(eventName: string, ...args: any): void;
    invoke(eventName: string, ...args: any): void;
};
declare const window: any;

@Component({
    selector: 'app-chat',
    templateUrl: './chat.component.html',
    styleUrls: ['./chat.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatComponent implements OnInit, OnDestroy {

    chatBodies: { name: string, addToOtherChatBodies: number[], messages: SafeHtml[] }[] = [
        { name: "Normal", addToOtherChatBodies: [1], messages: [] },
        { name: "Dirty", addToOtherChatBodies: [], messages: [] },
        { name: "Team", addToOtherChatBodies: [0, 1], messages: [] },
        { name: "Global", addToOtherChatBodies: [0, 1], messages: [] }
    ];

    infoAnimationPlayer: AnimationPlayer;
    infoTexts: string[] = [""];
    infoText: string;
    infoAnimationLastTimeMs: number;
    chatActive = true;
    chatInputActive = true;
    playerNames: string[] = [];

    selectedChatBody = 0;
    private scrollToBottomTimer: NodeJS.Timeout;
    isNearBottom = true;
    mentionShowing = false;

    private commandPrefix = "/";
    private maxMessagesInBody = 40;

    @ViewChild(MatInput, { static: true }) input: MatInput;
    @ViewChild("chatBody", { static: true }) chatBody: ElementRef;
    @ViewChild(MentionDirective, { static: true }) mentionDirective: MentionDirective;
    @ViewChild("marquee", { static: false }) infoSpan: ElementRef;

    mentionConfig: MentionConfig[] = [{
        items: this.playerNames,
        triggerChar: "@",
        mentionSelect: this.getMentionText,
        seachStringEndChar: ":",
        maxItems: 10
    }];

    private colorStrReplace = {
        "#r#": "rgb(222, 50, 50)",
        "#b#": "rgb(92, 180, 227)",
        "#g#": "rgb(113, 202, 113)",
        "#y#": "rgb(238, 198, 80)",
        "#p#": "rgb(131, 101, 224)",
        "#q#": "rgb(226, 79, 128)",
        "#o#": "rgb(253, 132, 85)",
        "#c#": "rgb(139, 139, 139)",
        "#m#": "rgb(99, 99, 99)",
        "#u#": "rgb(0, 0, 0)",
        "#s#": "rgb(220, 220, 220)",
        "#w#": "white",
        "#dr#": "rgb(169, 25, 25)"
    };

    constructor(
        private rageConnector: RageConnectorService,
        private changeDetector: ChangeDetectorRef,
        public settings: SettingsService,
        private sanitizer: DomSanitizer,
        private animationBuilder: AnimationBuilder
    ) { }

    ngOnInit() {
        if (typeof (mp) !== "undefined") {
            mp.events.add("chat:push", this.addMessage.bind(this));
            mp.events.add("chat:clear", this.clearChat.bind(this));
            mp.events.add("chat:activate", this.activateChat.bind(this));
            mp.events.add("chat:show", this.showChat.bind(this));

            window.chatAPI = {
                push: this.addMessage.bind(this),
                show: this.showChat.bind(this),
                activate: this.activateChat.bind(this),
                clear: this.clearChat.bind(this)
            };
        }

        this.rageConnector.listen(DFromClientEvent.AddNameForChat, this.addNameForChat.bind(this));
        this.rageConnector.listen(DFromClientEvent.LoadNamesForChat, this.loadNamesForChat.bind(this));
        this.rageConnector.listen(DFromClientEvent.RemoveNameForChat, this.removeNameForChat.bind(this));
        this.rageConnector.listen(DFromClientEvent.ToggleChatInput, this.toggleChatInput.bind(this));
        this.rageConnector.listen(DFromServerEvent.LoadChatInfos, this.loadChatInfos.bind(this));

        this.settings.ChatSettingsChanged.on(null, this.chatSettingsChanged.bind(this));
    }

    ngOnDestroy() {
        if (typeof (mp) !== "undefined") {
            mp.events.remove("chat:push", this.addMessage.bind(this));
            mp.events.remove("chat:clear", this.clearChat.bind(this));
            mp.events.remove("chat:activate", this.activateChat.bind(this));
            mp.events.remove("chat:show", this.showChat.bind(this));
        }

        this.rageConnector.remove(DFromClientEvent.AddNameForChat, this.addNameForChat.bind(this));
        this.rageConnector.remove(DFromClientEvent.LoadNamesForChat, this.loadNamesForChat.bind(this));
        this.rageConnector.remove(DFromClientEvent.RemoveNameForChat, this.removeNameForChat.bind(this));
        this.rageConnector.remove(DFromClientEvent.ToggleChatInput, this.toggleChatInput.bind(this));
        this.rageConnector.remove(DFromServerEvent.LoadChatInfos, this.loadChatInfos.bind(this));

        this.settings.ChatSettingsChanged.off(null, this.chatSettingsChanged.bind(this));
    }

    removeInputFocus() {
        if (this.mentionShowing)
            return;

        this.toggleChatInput(false);
        this.rageConnector.call(DToClientEvent.CloseChat);
    }

    selectChatBody(index: number) {
        this.selectedChatBody = index;
        this.changeDetector.detectChanges();

        if (this.scrollToBottomTimer) {
            clearTimeout(this.scrollToBottomTimer);
        }
        this.scrollToBottomTimer = setTimeout(this.scrollChatToBottom.bind(this), 500);
    }

    getMentionText(name: string): string {
        return "@" + name + ": ";
    }

    private addMessage(msg: string) {
        const addToBodies = this.getChatBodiesByMessage(msg);
        msg = this.removeChatBodyIndicatorInMessage(msg, addToBodies[0]);
        const mentionsMe = this.getMentionsMe(msg);

        const safeHtml = this.formatMessage(msg, mentionsMe);

        for (const bodyIndex of addToBodies) {
            this.chatBodies[bodyIndex].messages.push(safeHtml);
            if (this.chatBodies[bodyIndex].messages.length > this.maxMessagesInBody) {
                this.chatBodies[bodyIndex].messages.shift();
            }
        }

        if (addToBodies.indexOf(this.selectedChatBody) >= 0) {
            this.changeDetector.detectChanges();

            if (this.isNearBottom) {
                this.scrollChatToBottom();
            } else if (!this.scrollToBottomTimer) {
                this.scrollToBottomTimer = setTimeout(this.scrollChatToBottom.bind(this), 15000);
            }
        }
    }

    private clearChat() {
        for (const chatBody of this.chatBodies) {
            chatBody.messages = [];
        }
        this.changeDetector.detectChanges();
    }

    private activateChat(toggle: boolean) {
        this.chatInputActive = toggle;
        if (this.chatActive) {
            this.changeDetector.detectChanges();
        }
    }

    private showChat(toggle: boolean) {
        this.chatActive = toggle;
        this.chatInputActive = toggle;
        this.changeDetector.detectChanges();
    }

    private toggleChatInput(toggle: boolean, cmd: string = "") {
        if (!this.chatActive || !this.chatInputActive)
            return;

        if (typeof (mp) !== "undefined") {
            mp.invoke("setTypingInChatState", toggle);
        }
        this.settings.setChatInputOpen(toggle);
        this.input.value = cmd;
        this.scrollChatToBottom();
        this.changeDetector.detectChanges();

        if (toggle) {
            this.input.focus();
            this.changeDetector.detectChanges();
        } else {
            this.mentionDirective.closeSearchList();
        }
    }

    private isNullOrWhitespace(input: string) {
        return !input || !input.trim();
    }

    scrolled(event: any): void {
        this.isNearBottom = this.isChatScrolledToBottom();
    }

    private isChatScrolledToBottom(): boolean {
        const threshold = 150;
        const element = this.chatBody.nativeElement as HTMLDivElement;
        const position = element.scrollTop + element.offsetHeight;
        const height = element.scrollHeight;
        return position > height - threshold;
    }

    private scrollChatToBottom() {
        this.chatBody.nativeElement.scroll({
            top: this.chatBody.nativeElement.scrollHeight,
            left: 0,
            behavior: 'smooth'
        });
        this.isNearBottom = true;
        this.changeDetector.detectChanges();

        if (this.scrollToBottomTimer) {
            clearTimeout(this.scrollToBottomTimer);
            this.scrollToBottomTimer = undefined;
        }
    }

    private getChatBodiesByMessage(msg: string): number[] {
        let body = 0;
        for (let i = 0; i < this.chatBodies.length; ++i) {
            if (msg.endsWith("$" + this.chatBodies[i].name + "$")) {
                body = i;
                break;
            }
        }
        return [body, ...this.chatBodies[body].addToOtherChatBodies];
    }

    private removeChatBodyIndicatorInMessage(msg: string, body: number): string {
        if (!msg.endsWith("$" + this.chatBodies[body].name + "$"))
            return msg;

        return msg.slice(0, -("$" + this.chatBodies[body].name + "$").length);
    }

    private getMentionsMe(msg: string): boolean {
        if (msg.indexOf("@" + this.settings.Constants[7] + ":") >= 0
            || msg.indexOf("@" + this.settings.Constants[8] + ":") >= 0)
            return true;
        return false;
    }

    private formatMessage(msg: string, mentionsMe: boolean): SafeHtml {
        let start = "";
        if (mentionsMe) {
            start = '<span style="background-color: rgba(255,178,102,0.6);">';
        }
        start += '<span style="color: white;">';

        let replaced = this.replaceColorStr(msg);
        replaced = this.replaceRGBColor(replaced);

        if (mentionsMe)
            replaced += "</span>";

        return this.sanitizer.bypassSecurityTrustHtml(start + replaced + "</span>");
    }

    private replaceColorStr(msg: string): string {
        let hashtagIndex = msg.indexOf("#");
        let nextHashtagIndex = 0;
        while (hashtagIndex >= 0) {
            nextHashtagIndex = msg.indexOf("#", hashtagIndex + 1);
            if (nextHashtagIndex > 0) {
                const toReplaceColorStr = msg.substring(hashtagIndex, nextHashtagIndex + 1);
                if (this.colorStrReplace[toReplaceColorStr]) {
                    msg = msg.replace(toReplaceColorStr, "</span><span style='color: " + this.colorStrReplace[toReplaceColorStr] + ";'>");
                    hashtagIndex = msg.indexOf("#", nextHashtagIndex + 1);
                } else if (toReplaceColorStr === "#n#") {
                    msg = msg.replace(/#n#/g, '<br>');
                } else {
                    hashtagIndex = nextHashtagIndex;
                }
            } else {
                break;
            }
        }

        return msg;
    }

    private replaceRGBColor(msg: string): string {
        let index = msg.indexOf("!$");
        while (index >= 0) {
            const endindex = msg.indexOf("$", index + 2);
            if (endindex == -1)
                break;
            const colorstr = msg.substring(index, endindex + 1);
            let rgbarr: string[];
            if (colorstr.indexOf(",") >= 0)
                rgbarr = colorstr.substring(2, colorstr.length - 1).split(",");
            else
                rgbarr = colorstr.substring(2, colorstr.length - 1).split("|");
            let rgbcolor: string;
            if (rgbarr.length == 1)
                rgbcolor = rgbarr[0];
            else if (rgbarr.length < 3)
                rgbcolor = `rgb(${(0 in rgbarr ? rgbarr[0] : 0)}, ${(1 in rgbarr ? rgbarr[1] : 0)}, 0)`;
            else if (rgbarr.length == 3)
                rgbcolor = `rgb(${rgbarr[0]}, ${rgbarr[1]}, ${rgbarr[2]})`;
            else if (rgbarr.length == 4)
                rgbcolor = `rgba(${rgbarr[0]}, ${rgbarr[1]}, ${rgbarr[2]}, ${rgbarr[3]})`;
            const replacement = "</span><span style='color: " + rgbcolor + ";'>";
            msg = msg.replace(colorstr, replacement);
            index = msg.indexOf("!$", index + replacement.length);
        }
        return msg;
    }

    private addNameForChat(name: string) {
        console.log("addNameForChat 1 | name: " + name + " | playerNames: " + this.playerNames);
        this.playerNames.push(name);
        this.mentionDirective.refreshItems(this.mentionConfig[0].items);
        console.log("addNameForChat 2");
        this.changeDetector.detectChanges();
    }

    private loadNamesForChat(namesJson: string) {
        console.log("loadNamesForChat 1 | playerNames: " + this.playerNames);
        this.playerNames = JSON.parse(namesJson);
        this.mentionConfig[0].items = this.playerNames;
        this.mentionDirective.refreshItems(this.mentionConfig[0].items);

        console.log("loadNamesForChat 2 | playerNames: " + this.playerNames);
        this.changeDetector.detectChanges();
    }

    private removeNameForChat(name: string) {
        console.log("removeNameForChat 1 | name: " + name + " | playerNames: " + this.playerNames);
        const index = this.playerNames.indexOf(name);
        if (index >= 0) {
            this.playerNames.splice(index, 1);
        }
        this.mentionDirective.refreshItems(this.mentionConfig[0].items);
        console.log("removeNameForChat 2 | playerNames: " + this.playerNames);
        this.changeDetector.detectChanges();
    }

    private chatSettingsChanged() {
        if (this.chatBodies[this.selectedChatBody].name === "Dirty" && this.settings.ChatHideDirtyChat) {
            this.selectChatBody(0);
        }
        if (this.infoAnimationLastTimeMs && this.infoAnimationLastTimeMs != this.settings.ChatInfoAnimationTimeMs) {
            this.infoAnimationPlayer.destroy();
            this.createInfoAnimation();
        }

        this.changeDetector.detectChanges();
    }

    onMentionShowingChanged(showing: boolean) {
        this.mentionShowing = showing;
        this.changeDetector.detectChanges();
    }

    loadChatInfos(datasJson: string) {
        this.infoTexts = JSON.parse(datasJson);
        if (this.infoTexts.length == 0) {
            this.infoText = undefined;
            this.changeDetector.detectChanges();
            return;
        }

        this.createInfoAnimation();
        this.setNextInfo();
    }

    createInfoAnimation() {
        if (!this.infoSpan) {
            this.changeDetector.detectChanges();
        }

        this.infoAnimationLastTimeMs = this.settings.ChatInfoAnimationTimeMs;
        const animation = this.animationBuilder.build([
            style({ transform: 'translateX(0)' }),
            animate(this.settings.ChatInfoAnimationTimeMs, style({ transform: 'translateX(-100%)' }))
        ]);
        this.infoAnimationPlayer = animation.create(this.infoSpan.nativeElement);
        // Does not work (https://github.com/angular/angular/issues/26630)
        // this.infoAnimationPlayer.onDone = this.setNextInfo.bind(this);
        this.infoAnimationPlayer.init();
    }

    setNextInfo() {
        if (!this.infoText) {
            this.setInfoText(this.infoTexts[0]);
            return;
        }

        const currentIndex = this.infoTexts.indexOf(this.infoText);
        if (currentIndex < 0) {
            this.setInfoText(this.infoTexts[0]);
            return;
        }

        const nextIndex = currentIndex + 1 >= this.infoTexts.length ? 0 : currentIndex + 1;

        this.setInfoText(this.infoTexts[nextIndex]);
    }

    setInfoText(text: string) {
        this.infoText = text;
        this.infoAnimationPlayer.restart();
        setTimeout(this.setNextInfo.bind(this), this.settings.ChatInfoAnimationTimeMs);
        this.changeDetector.detectChanges();
    }

    @HostListener('window:keydown', ['$event'])
    keyEvent(event: KeyboardEvent) {
        if (event.key === "Enter" && this.settings.ChatInputOpen && !this.mentionShowing) {
            event.preventDefault();
            let msg = this.input.value;
            if (!this.isNullOrWhitespace(msg) && msg !== this.commandPrefix) {
                // msg = msg.replace(/\\/g, "\\\\").replace(/\"/g, "\\\"");
                if (msg.startsWith(this.commandPrefix)) {
                    msg = msg.substr(this.commandPrefix.length);
                    this.rageConnector.call(DToClientEvent.CommandUsed, msg);
                } else {
                    this.rageConnector.call(DToClientEvent.ChatUsed, msg, this.selectedChatBody);
                }
            } else {
                this.rageConnector.call(DToClientEvent.CloseChat);
            }
            this.mentionDirective.closeSearchList();
        } else if (event.key === " " && !this.settings.ChatInputOpen && (event.target as HTMLDivElement).id == "chat_container") {
            event.preventDefault();
        }




        /*else if (event.key === "ArrowLeft") {
            this.toLeft();
        } else if (event.key === "Enter") {
            this.buttons[this.buttons.length - this.selectedIndex - 1].func();
        } else if (!isNaN(parseInt(event.key, 10))) {
            const index = parseInt(event.key, 10);
            if (this.buttons.length >= index) {
                this.buttons.find(i => i.index == index - 1).func();
            }
        } else {
            this.stopTurnAroundAnimation();
        }*/
    }
}
