import { injectable, inject } from "inversify";
import LoggingService from "../output/logging.service";

@injectable()
export default class CharCreatorService {

    constructor(
        @inject(LoggingService) logging: LoggingService
    ) {
        
    }
}
