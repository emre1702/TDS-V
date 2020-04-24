export interface MentionConfig {
    items: string[];
    triggerChar: string;
    seachStringEndChar: string;
    mentionSelect: (item: string) => string;
    maxItems?: number;
    dropUp?: boolean;
}
