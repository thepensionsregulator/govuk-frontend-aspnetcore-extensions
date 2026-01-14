import { IUmbracoProperty } from "./IUmbracoProperty";

export interface IBlockListItem {
    key: string;
    contentTypeKey: string;
    values: Array<IUmbracoProperty>;
}
