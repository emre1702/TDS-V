import game from "natives";
import alt from "alt-client";
import { logErrorToServer } from "./logging.helper";
import { languagesDict } from "../constants";
import LanguageValue from "../enums/output/language-value.enum";

export function loadModelAsync(model: number | string): Promise<number> {
    return new Promise((resolve, reject) => {
        if (typeof model === "string") {
            model = game.getHashKey(model);
        }

        if (!game.isModelValid(model)) {
            logErrorToServer(languagesDict[LanguageValue.English].INVALID_MODEL.format(model), "loadModelAsync failed");
            return reject("InvalidModelInfo");
        }

        if (game.hasModelLoaded(model)) {
            return resolve(model);
        }

        game.requestModel(model);
        const interval = alt.setInterval(() => {
            if (game.hasModelLoaded(model as number)) {
                alt.clearInterval(interval);
                return resolve(model as number);
            }
        }, 0);
    })
        
}
