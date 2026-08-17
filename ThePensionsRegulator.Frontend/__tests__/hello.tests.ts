import { hello } from "../Scripts/hello";

test("says hello", () => expect(hello("World")).toBe("Hello, World!"));