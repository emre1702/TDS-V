export interface MentionConfig {
    items: any[];
    triggerChar: string;
    seachStringEndChar: string;
    mentionSearch: (item: any, str: string) => boolean;
    mentionSelect: (item: any) => string;
    mentionInfo: (item: any) => string;
    mentionSelectedInfo: (info: any) => string;
    maxItems?: number;
    dropUp?: boolean;
    onlyAllowAtBeginning?: boolean;
}
