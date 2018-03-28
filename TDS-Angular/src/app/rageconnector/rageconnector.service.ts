import { Injectable, EventEmitter } from "@angular/core";

declare var window: any;

export class RAGE {
    
    private CustomEvents: EventEmitter<RAGEEvent> = new EventEmitter<RAGEEvent>(); 

    constructor() {
        // used in rageconnector.js //
        window.RAGE = {
            call: this.call,
            Client: {
                response: this.Client.response,
            }
        };
        window.RageAngular = {};

        if (typeof window.RageJS === "object") {
            window.RageJS.Init();
        }
    }

    public Client: RAGEClient = {
        /**
		 * This array contain all functions which are waiting for a response from the client
         */
        waitingResponses: [],
        idCounter: 0,

        /**
		 * This function is used to call the client (mp.trigger)
         * @param {RAGEClientCall} data - Here it will be the function name/id and the arguments
         * @param {RAGEClientCallback} [callback] - This is the function which it will be called when the service receive a response from the client
         */
        call: (data: RAGEClientCall, callback?: (response: string | number | object) => void) => {
            if ( typeof window.RageJS.callClient !== "function") throw new Error("RageJS.callClient isn't set in index.html");

            // We prepare the data for the client
            let to_client: any = {
                Arguments: data.args,
            };

            // If the callback function is defined, we will put the callback function in the waitingResponses
            // And when we receive a response, we will call the callback function
            if (typeof callback === "function") {
                // It's necessary, if 2+ functions are called in the same time?
                let id = this.Client.addToWaitingResponses(data.fn, callback);

                to_client.catchResponse = {
                    id: id,
                    fnCalled: data.fn,
                };
            }

            // After we managed the data, we send to client
            window.RageJS.callClient(data.fn, to_client);
        },

        /**
         * This function adds the function to use from Rage.
         * @param {any} instance - Instance where the method is
         * @param {any} func - The function to be called from Rage.
         * @param {string} name - Your desired name for the function to be used when using angularbrowser.call from Rage
         */
        listen: (instance: any, func: any, name: string): void => {
            window.RageAngular[name] = func.bind(instance);
        },

        /**
		 * This function is used to filter all responses from the client and call the callback function
         * @param {number} id - The unique id from waiting responses
         * @param {string|number|Object} response - The response from the client
         */
        response: (id: number, response: string | number | object) => {
            // Getting the index from waitingResponses
            let index = this.Client.getWaitingResponseIndex(id);
            if (index === -1) throw new Error("RAGE ERROR: A response came from client but the function wasn't in waiting responses");

            // Calling the callback function
            this.Client.waitingResponses[index].callback(response);
            // Remove the entry 
            this.Client.waitingResponses.splice(index, 1);
        },

        /**
		 * This function is used to search the index from waitingResponses based on the function name/id
         * @param {number} id - The unique id
		 * @returns {number} The index from waitingResponses
         */
        getWaitingResponseIndex: function(id: number): number {
            let length = this.waitingResponses.length;
            let index = -1;

            for (let i = 0; i < length; ++i) {
                if ( this.waitingResponses[i].id === id ) {
                    index = i;
                    break;
                }
            }

            return index;
        },

        /**
		 * This function register in the waitingResponses the callback function
         * @param {string|number} func - The function name/id
         * @param {RAGEClientCallback} callback - The callback function
         * @returns {number} The unique id (not index) from waitingResponses
         */
        addToWaitingResponses: (func: string | number, callback: (response: string | number | object) => void): number => {
            let id = ++this.Client.idCounter;

            this.Client.waitingResponses.push({
                id: id,
                fnCalled: func,
                callback: callback,
            });

            return id;
        }
    };

    private call: Function = (func: string | number, ...args: any[]): void  => {
        this.CustomEvents.emit({
            func: func,
            args: args
        });
    }

    /*get getListen(): EventEmitter<RAGEEvent> {
        return this.CustomEvents;
    }*/
}

export interface RAGEClient {
    waitingResponses: Array<RAGEWaitingResponse>;
    idCounter: number;
    call: (data: RAGEClientCall, callback?: (response: string | number | object) => void) => void;
    listen: (instance: any, func: any, name?: string) => void;
    response: (id: number, fn: string | number, response: string | number | object) => void;
    getWaitingResponseIndex: (id: number) => number;
    addToWaitingResponses: (func: string | number, callback: (response: string | number | object) => void) => number;
}

/**
 * The object with client call data
 * @typedef {Object} RAGEClientCall
 * @property {string|number} fn - The function called
 * @property {(number|string|object)[]} args - The arguments
 */
export interface RAGEClientCall {
    fn: string | number;
    args: Array<number | string>;
}

/**
 * The object which is in waitingResponses
 * @typedef {Object} RAGEWaitingResponse
 * @property {number} id - The unique id
 * @property {string|number} fnCalled - The function name/id
 * @property {RAGEClientCallback} callback - The function callback
 */
export interface RAGEWaitingResponse {
    id: number;
    fnCalled: string | number;
    callback: (response: string | number | object) => void;
}

export interface RAGEEvent {
    func: string | number;
    args: any[];
}