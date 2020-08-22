import { injectable, inject } from "inversify";
import LoggingService from "../../datas/helper/logging.helper";

@injectable()
export default class CharCreatorService {

    constructor(
        @inject(LoggingService) logging: LoggingService
    ) {
        
    }
}
