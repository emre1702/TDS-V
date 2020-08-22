import Language from "../../interfaces/language.interface";

export default class English implements Language {
    InvalidModel = "The model '{0}' of an object is invalid and could not be loaded.";
    InvalidModelInfo = "The model of an object is invalid and could not be loaded. The developers are informed.";


    get(index: any): string {
        return (this as any)[index];
    }
}
