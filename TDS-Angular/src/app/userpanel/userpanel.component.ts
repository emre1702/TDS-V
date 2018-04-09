import { Component, ChangeDetectorRef, OnInit, ViewChild, Type, ComponentFactoryResolver } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";

import { UserpanelContentDirective } from "./content/userpanelcontent.directive";
import { RAGE } from "../rageconnector/rageconnector.service";
import { AppComponent } from "../app.component";
import { UserpanelRulesComponent } from "./rules/rules.component";
import { UserpanelSettingsComponent } from "./settings/settings.component";
import { UserpanelReportsComponent } from "./reports/reports.component";
import { UserpanelSuggestionsComponent } from "./suggestions/suggestions.component";
import { UserpanelAdminComponent } from "./admin/admin.component";
import { UserpanelDonatorComponent } from "./donator/donator.component";

@Component({
  selector: "app-userpanel",
  templateUrl: "./userpanel.component.html",
  styleUrls: ["./userpanel.component.css"]
})

export class UserpanelComponent implements OnInit {
    private static openedMenu;
    language = UserpanelContentData.getLang("main");
    @ViewChild(UserpanelContentDirective) userpanelContentDirective: UserpanelContentDirective;
    // private components: UserpanelContentItem[] = [];
    // @ViewChild("sidenav") sidenav: MatSidenav;
    // private currentOpenedComponent: UserpanelContentComponent;
    static instance: UserpanelComponent;

    constructor ( private rage: RAGE, private chdref: ChangeDetectorRef, private componentFactoryResolver: ComponentFactoryResolver ) {
        if ( !UserpanelComponent.instance ) {
            rage.Client.listen( UserpanelReportsComponent.syncReport, "syncReport" );
            rage.Client.listen( UserpanelReportsComponent.syncReports, "syncReports" );
            rage.Client.listen( UserpanelReportsComponent.syncReportText, "syncReportText" );
            rage.Client.listen( UserpanelReportsComponent.syncReportTexts, "syncReportTexts" );
            rage.Client.listen( UserpanelReportsComponent.syncReportState, "syncReportState" );
            rage.Client.listen( UserpanelReportsComponent.syncReportRemove, "syncReportRemove" );
        } 
        UserpanelComponent.instance = this;           
    }

    ngOnInit() {
        UserpanelComponent.changeMenu ( 0 );  
    }

    static changeMenu ( index: number ) {
        if ( index < UserpanelContentData.menuNames.length ) {
            UserpanelComponent.instance.loadComponent( index );

            UserpanelComponent.openedMenu = UserpanelContentData.menuNames[index]; 
        }
    }
    changeMenu = UserpanelComponent.changeMenu;

    static close() {
        /*this.rage.Client.call ( {
            fn: "closeUserpanel",
            args: []
        } );*/
        AppComponent.closeMenu( "userpanel" );
    }
    close = UserpanelComponent.close;

    loadComponent ( index: number ) {
        let componentname = UserpanelContentData.menuNames[index];
        let component: Type<any> = UserpanelContentData.menus[componentname];
        
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(component);

        let viewContainerRef = this.userpanelContentDirective.viewContainerRef;
        viewContainerRef.clear();
    
        let componentRef = viewContainerRef.createComponent(componentFactory);
        componentRef.instance.language = UserpanelContentData.getLang( componentname );
        if ( componentRef.instance.neededAdminlvls !== null ) { 
            componentRef.instance.myAdminlvl = AppComponent.Settings.adminLvl;
            componentRef.instance.neededAdminlvls = UserpanelContentData.neededAdminlvls[componentname];
        }
    }

    get menuNames() {
        return UserpanelContentData.menuNames;
    }

    get openedMenu() {
        return UserpanelComponent.openedMenu;
    }
}



class UserpanelContentData {
    static menuNames = [
        // stats,
        "rules",
        // "settings",
        "reports",
        // "suggestions",
        // "admin",
        // "donator"
    ];
    static menus = {
        // stats: UserpanelStatsComponent, 
        rules: UserpanelRulesComponent,
        settings: UserpanelSettingsComponent,
        reports: UserpanelReportsComponent,
        suggestions: UserpanelSuggestionsComponent,
        admin: UserpanelAdminComponent,
        donator: UserpanelDonatorComponent
    };
    static neededAdminlvls = {
        reports: {
            removeReport: 2
        }
    };

    static language = {
        ENGLISH: {
            main: {
                stats: "Stats",
                reports: "Reports",
                settings: "Settings",
                admin: "Admin",
                donator: "Donator",
                suggestions: "Suggestions",
                rules: "Rules"
            },

            rules: {
                normal: "Normal",
                team: "Team",

                normalrules: [
                    `Teaming is not allowed!<br>
                    At TDS the term teaming describes atleast two opponents not fighting each other and possibly endangering the own team-mates.<br>  
                    Exception: Every alive player in the round agreed or you play in the 2. Arena-lobby.`,

                    `Insulting in normal-chat is prohibited.<br>
                    If you want to insult or read other insults, you should change to dirty-chat.`,

                    `Intentionally trying to harm the server is forbidden!<br>
                    You can mention other server and praise them, you can criticize TDS and maybe insult the server on rage (dirty-Chat).<br>
                    But if it's clear that you only try to harm the server, your behaviour could get punished.`,
                ],
                teamrules: [
                    `Exploiting the admin-rights is strictly forbidden!<br>
                    Muten, kicking etc. yourself is exploiting the right.<br>
                    Punishing a user harder or without him breaking a rule can have serious consequences for you. `,

                    `Insults against team-members have to be handled the same as if they were against a normal user.<br>
                    You have to stay calm and treat the user fair.`,

                    `Only the project-leading is allowed to punish teaming.<br>
                    If you noticed someone teaming you needed a 100% clear evidence which you can send to the project-leading.
                    Only if the proof shows the teaming exactly, something will be done - else not.`,

                    `If a user wants to clarify (after a punishment), you have to agree atleast once and listen to the user while staying friendly.<br>`,

                    `For you insults against the server are prohibited!<br>
                    If you want to criticize the server you can do it at one of the project-leading.<br>
                    But you should never denigrate the server - in which you are a member - in public.`,

                    `You should always act as a role model und be friendly to user.<br>
                    So you shouldn't insult, be salty, flame or act arrogant.<br>
                    The friendlier you are to the user, the better is it for the server.`
                ]
            },

            reports: {
                owner: "Owner",
                projectmanager: "Project-manager",
                administrator: "Administrator",
                supporter: "Supporter",

                title: "Title",
                state: "State",
                create: "Create",
                view: "View",
                open: "Open",
                close: "Close",
                remove: "Remove",

                for_teammember: "For (min.):",
                answer: "Answer",
                send: "Send"
            }
        },
        GERMAN: {
            main: {
                stats: "Statistiken",
                reports: "Reports",
                settings: "Einstellungen",
                admin: "Admin",
                donator: "Spender",
                suggestions: "Vorschläge",
                rules: "Regeln"
            },

            rules: {
                normal: "Normal",
                team: "Team",

                normalrules: [
                    `Teamen ist nicht erlaubt!<br>
                    Unter Teamen versteht man auf TDS, wenn min. zwei von gegnerischen Teams sich extra nicht abschießen und evtl. somit die Leben der eigenen Team-Mitglieder gefährden.<br>  
                    Ausnahme: Alle lebenden Spieler in der Runde sind einverstanden oder es wird in der 2. Arena-Lobby gespielt.`,

                    `Beleidigungen im normalen Chat sind nicht gestattet!<br>
                    Wenn ihr beleidigen und/oder Beleidigungen lesen wollt, wechselt in den dirty-Chat.`,

                    `Das absichtliche Schädigen des Servers ist verboten!<br>
                    Ihr dürft Server nennen und sie loben, ihr dürft TDS kritisieren und bei Wut auch mal beleidigen (dirty-Chat).<br>
                    Wenn jedoch klar wird, dass ihr lediglich versucht dem Server zu schaden, kann das Verhalten bestraft werden.`,
                ],
                teamrules: [
                    `Ausnutzung der Rechte sind strengstens verboten.<br>
                    Sich selbst muten, kicken o.ä. zählt als Ausnutzung.<br>
                    User schwerer oder ohne Regelverstoß bestrafen wegen persönlichen Gründen kann schwere Folgen haben.`,

                    `Beleidigungen gegen Team-Mitglieder sind genauso zu handhaben wie als wären sie gegen normale User.<br>
                    Eure Pflicht ist es dabei gelassen zu bleiben und fair zu handeln.`,

                    `Nur die Projektleitung darf für Teamen bestrafen.<br>
                    Wenn ihr Spieler beim Teamen erwischt, braucht ihr 100% eindeutige Beweise, die ihr der Projektleitung vorlegen könnt.<br>
                    Nur wenn klar erkennbar ist, dass Spieler geteamt haben, wird da was unternommen.`,

                    `Bei Klärungsversuch des Users muss min. einmal zugestimmt und der User freundlich angehört werden.<br>`,

                    `Für euch ist Server-Beleidigung strengstens verboten.<br>
                    Bei Kritik könnt ihr euch bei der Projektleitung melden, jedoch solltet ihr öffentlich niemals den Server schlechtmachen, dem ihr als Mitglied angehört.`,

                    `Ihr solltet immer als Vorbilder agieren und freundlich zu Usern sein.<br>
                    Daher solltet ihr nicht beleidigen oder flamen.<br>
                    Verhaltet euch nicht herablassend oder aufgeblasen.<br>
                    Je freundlicher ihr mit den Usern umgeht, desto besser ist es für den Server.`
                ]
            },

            reports: {
                owner: "Besitzer",
                projectmanager: "Projekt-Manager",
                administrator: "Administrator",
                supporter: "Supporter",

                title: "Titel",
                state: "Status",
                create: "Erstellen",
                view: "Betrachten",
                open: "Öffnen",
                close: "Schließen",
                remove: "Entfernen",

                for_teammember: "Für (min.):",
                answer: "Antwort",
                send: "Senden"
            }
        } 
    };

    static getMenuNames() {
        return UserpanelContentData.menuNames; 
    }

    static getLang( menu: string ) {
        return UserpanelContentData.language[AppComponent.Settings.myLanguage][menu];
    }
}

