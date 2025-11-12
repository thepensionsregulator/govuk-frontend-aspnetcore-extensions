import { ILinkPickerModel } from './ILinkPickerModel';

export interface ITprDocumentBlock {
    key: string;
    docName: string | undefined;
    docLink: ILinkPickerModel | undefined;
    datePublished: Date | null;
    numberOfPages: number | undefined;
    description: string | undefined;
    ext: string | undefined;
}
