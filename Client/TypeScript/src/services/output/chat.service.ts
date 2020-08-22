import { inject, injectable } from "inversify";
import BrowsersService from "../browsers/browsers.service";
import ToBrowserEvent from "../../datas/enums/events/to-browser-event.enum";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";

@injectable()
export default class ChatService {

    constructor(
        @inject(DIIdentifier.BrowsersService) private browsersService: BrowsersService
    ) {}

    output(msg: string) {
        this.browsersService.angular.execute(ToBrowserEvent.ChatOutput, msg);
    }

    clear() {
        this.browsersService.angular.execute(ToBrowserEvent.ChatClear);
    }

    toggleInput(toggle: boolean) {
        this.browsersService.angular.execute(ToBrowserEvent.ChatInputShow, toggle);
    }

    toggle(toggle: boolean) {
        this.browsersService.angular.execute(ToBrowserEvent.ChatShow, toggle);
    }
}
