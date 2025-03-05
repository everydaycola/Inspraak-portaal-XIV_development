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
                index: resolve(__dirname, 'src/ts/index.ts'),
                site: resolve(__dirname, 'src/ts/site.ts'),
                validation: resolve(__dirname, 'src/ts/validation.ts')
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
