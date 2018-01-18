declare interface MpStorage {
    flush(): void;
    data: { [key: string]: any };
}