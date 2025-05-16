import { dirname, resolve } from 'path';
import { defineConfig } from 'vite';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

export default defineConfig({
    base: '/dist/',
    build: {
        sourcemap: true,
        outDir: resolve(__dirname, '..', 'wwwroot', 'dist'),
        emptyOutDir: true,
        rollupOptions: {
            input: {
                site: resolve(__dirname, 'src/ts/site.ts'),
                register: resolve(__dirname, 'src/ts/register/register.ts'),
                panelCreation: resolve(__dirname, 'src/ts/panel/panelCreationEntrypoint.ts'),
                apiDataFiller: resolve(__dirname, 'src/ts/panel/apiAutofill/apiDataFiller.ts'),
                adminOrgMan: resolve(__dirname, 'src/ts/organisationmanagement/adminOrgManEntrypoint.ts'),
                autoOpenModals: resolve(__dirname, 'src/ts/auto-open-modals.ts'),
                accountSettings: resolve(__dirname, 'src/ts/accountSettings/accountSettings.ts')
            },
            output: {
                entryFileNames: '[name].entry.js',
                assetFileNames: (assetInfo) => {
                    if (assetInfo.name.endsWith('.css')) {
                        return '[name].css';
                    }
                    return '[name].[ext]';
                }
            }
        }
    },
    resolve: {
        extensions: ['.ts', '.js']
    },
    css: {
        preprocessorOptions: {
            scss: {
                // scss options if needed
            }
        }
    },
});
