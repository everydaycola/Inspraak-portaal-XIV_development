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
                panelCreation: resolve(__dirname, 'src/ts/panelCreationPage/panelCreationEntrypoint.ts'),
                organisationManagement: resolve(__dirname, 'src/ts/organisationManagementPage/organisationManagementEntrypoint.ts'),
                accountSettings: resolve(__dirname, 'src/ts/accountSettings/accountSettingsEntrypoint.ts'),
                projectPage: resolve(__dirname, 'src/ts/projectPage/projectPageEntrypoint.ts'),
                people: resolve(__dirname, 'src/ts/peoplePage/peoplePageEntrypoint.ts'),
                site: resolve(__dirname, 'src/ts/siteEntrypoint.ts'),
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
