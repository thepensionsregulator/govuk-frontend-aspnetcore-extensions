import { goodbye } from "../Scripts/goodbye";

test("says goodbye", () => expect(goodbye("World")).toBe("Goodbye, World!"));