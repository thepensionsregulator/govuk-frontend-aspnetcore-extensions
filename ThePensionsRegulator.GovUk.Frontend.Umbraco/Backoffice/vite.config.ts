import { defineConfig } from "vite";
import { copyFileSync } from "fs";
import { resolve } from "path";

export default defineConfig({
  build: {
    lib: {
      entry: "src/manifests.ts",
      formats: ["es"],
      fileName: "manifest",
    },
    outDir: "../wwwroot/App_Plugins/ThePensionsRegulator.GovUk.Frontend.Umbraco", 
    emptyOutDir: true,
    sourcemap: true,
    rollupOptions: {
      external: [/^@umbraco/],
    },
    },
    plugins: [
        {
            name: 'copy-tinymce-plugin',
            closeBundle() {
                const src = resolve(__dirname, 'src/property-editors/tinymce/paste-from-word.min.js');
                const dest = resolve(__dirname, '../wwwroot/App_Plugins/ThePensionsRegulator.GovUk.Frontend.Umbraco/paste-from-word.min.js');
                copyFileSync(src, dest);
                console.log('Copied paste-from-word.min.js to output directory');
            }
        }
    ]
});