import { encode, decode, decodeAsync } from "@msgpack/msgpack"

export function fromServer<T>(bytes: Uint8Array): T {
    return decode(bytes) as T;
}

export function fromServerAsync<T>(bytes: Uint8Array): Promise<T> {
    return decodeAsync(bytes) as Promise<T>;
}
