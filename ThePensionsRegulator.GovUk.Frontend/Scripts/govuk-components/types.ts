export interface ButtonOptions {
  labelText: string;
  variant?: "primary" | "secondary" | "warning" | "secondary-warning";
  attributes?: Record<string, string>;
  type: HTMLButtonElement["type"];
}
