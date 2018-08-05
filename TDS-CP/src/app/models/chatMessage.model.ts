export class ChatMessage {
    public sent: Date;

    constructor(public username: string = "", public message: string = "", date: string = "") {
        this.sent = new Date(date);
    }
}
