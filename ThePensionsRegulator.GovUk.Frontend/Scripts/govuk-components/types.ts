export interface ButtonOptions {
  labelText: string;
  variant?: "primary" | "secondary" | "warning" | "secondary-warning";
  attributes?: Record<string, string>;
  type: HTMLButtonElement["type"];
}

type FormControl =
    | HTMLInputElement
    | HTMLSelectElement

export interface LabelOptions {
  labelText: string;
  htmlFor: string;
  attributes?: Record<string, string>;
}

export interface HintOptions {
  hintText: string;
  id: string;
  attributes?: Record<string, string>;
}

export type InputWidth = 
    |"xx-small" 
    | "x-small" 
    | "small" 
    | "medium"
    | "large"
    | "x-large"
    | "xx-large";

export interface TextInputOptions {
  id: string;
  name: string;
  type?: HTMLInputElement["type"];
  value?: string;
  width?: InputWidth;
  attributes?: Record<string, string>;
}

export interface TextInputFormGroupOptions {
  labelText: string;
  hintText?: string;
  input: TextInputOptions;
}

export interface FormGroupOptions {
    control: FormControl,
    label: HTMLLabelElement,
  hint?: HTMLDivElement,
}