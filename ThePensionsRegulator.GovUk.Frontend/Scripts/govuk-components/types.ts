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

export type FormControlWidth = 
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
  width?: FormControlWidth;
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

export interface Option {
    value: string;
    text: string;
    disabled?: boolean;
    attributes?: Record<string, string>;
}

export interface SelectOptions {
    id: string;
    name: string;
    value?: string;
    options: Option[];
    attributes?: Record<string, string>;
    width?: FormControlWidth;
}

export interface SelectFormGroupOptions {
    labelText: string;
    hintText?: string;
    select: SelectOptions;
}

export type LegendSize = "small" | "medium" | "large" | "x-large";

export interface FieldsetOptions {
    legendText: string;
    legendSize?: LegendSize;
    children: HTMLElement[];
    attributes?: Record<string, string>;
}