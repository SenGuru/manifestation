import { FlatCompat } from "@eslint/eslintrc";
import _import from "eslint-plugin-import";
import { fileURLToPath } from "node:url";
import { fixupPluginRules } from "@eslint/compat";
import js from "@eslint/js";
import path from "node:path";
import tsParser from "@typescript-eslint/parser";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const compat = new FlatCompat({
    baseDirectory: __dirname,
    recommendedConfig: js.configs.recommended,
    allConfig: js.configs.all
});

export default [{
    ignores: ["**/app-model.ts", "**/app-http-client.ts", "**/app-custom-enums.ts"],
}, {
    languageOptions: {
        parser: tsParser,
        ecmaVersion: 5,
        sourceType: "script",

        parserOptions: {
            project: "./tsconfig.json",
        },
    },
}, ...compat.extends(
    "eslint:recommended",
    "plugin:@typescript-eslint/recommended",
    "plugin:@angular-eslint/recommended",
    "plugin:@angular-eslint/template/process-inline-templates",
).map(config => ({
    ...config,
    files: ["**/*.ts"],
})), {
    files: ["**/*.ts"],

    plugins: {
        import: fixupPluginRules(_import),
    },

    rules: {
        "sort-imports": ["error", {
            ignoreCase: false,
            ignoreDeclarationSort: false,
            ignoreMemberSort: false,
            memberSyntaxSortOrder: ["none", "all", "multiple", "single"],
            allowSeparatedGroups: false,
        }],
        "no-multi-spaces": "error",
        "space-in-parens": ["error", "never"],
        'no-multiple-empty-lines': ['error', { max: 1 }],
        "space-before-function-paren": ["error", "never"],
        "space-before-blocks": ["error", "always"],
        "keyword-spacing": ["error", { "before": true, "after": true }],
        "space-infix-ops": "error",
        "import/no-duplicates": "error",
        "arrow-body-style": "error",
        "@typescript-eslint/consistent-type-assertions": "error",
        "@typescript-eslint/consistent-type-definitions": "error",
        "@typescript-eslint/no-explicit-any": "error",
        "@typescript-eslint/explicit-function-return-type": "error",
        "@typescript-eslint/explicit-member-accessibility": ["error", { "accessibility": "explicit" }],
        "@typescript-eslint/explicit-module-boundary-types": "error",
        "@typescript-eslint/member-ordering": "error",
        "@typescript-eslint/method-signature-style": "error",
        "@typescript-eslint/no-duplicate-enum-values": "error",
        "@typescript-eslint/no-duplicate-type-constituents": "error",
        "@typescript-eslint/no-dynamic-delete": "error",
        "@typescript-eslint/no-empty-function": "error",
        "@typescript-eslint/no-empty-interface": "error",
        "@typescript-eslint/no-extra-non-null-assertion": "error",
        "@typescript-eslint/no-for-in-array": "error",
        "@typescript-eslint/no-invalid-void-type": "error",
        "@typescript-eslint/no-misused-new": "error",
        "@typescript-eslint/no-non-null-assertion": "error",
        "@typescript-eslint/no-require-imports": "error",
        "@typescript-eslint/no-this-alias": "error",
        "@typescript-eslint/no-throw-literal": "error",
        "@typescript-eslint/no-unused-vars": "warn",
        "@typescript-eslint/no-unsafe-argument": "error",
        "@typescript-eslint/no-unsafe-assignment": "error",
        "@typescript-eslint/no-unsafe-call": "error",
        "@typescript-eslint/no-unsafe-declaration-merging": "error",
        // "@typescript-eslint/no-unsafe-enum-comparison": "error",
        "@typescript-eslint/no-unsafe-member-access": "warn",
        "@typescript-eslint/no-unsafe-return": "error",
        "@typescript-eslint/no-unused-expressions": "error",
        "@typescript-eslint/no-use-before-define": "error",
        "@typescript-eslint/no-useless-empty-export": "error",
        // "@typescript-eslint/no-useless-template-literals": "error",
        "@typescript-eslint/prefer-as-const": "error",
        "@typescript-eslint/typedef": "error",
        "no-console": "error",

        "@angular-eslint/directive-selector": ["error", {
            type: "attribute",
            prefix: "app",
            style: "camelCase",
        }],

        "@angular-eslint/component-selector": ["error", {
            type: "element",
            prefix: "app",
            style: "kebab-case",
        }],
    },
}, ...compat.extends("plugin:@angular-eslint/template/recommended").map(config => ({
    ...config,
    files: ["**/*.html"],
})), {
    files: ["**/*.html"],

    rules: {
        // "@angular-eslint/template/interactive-supports-focus": "warn",
        "@angular-eslint/template/click-events-have-key-events": "warn",
        "@angular-eslint/template/alt-text": "error",
        "@angular-eslint/template/attributes-order": "error",
        "@angular-eslint/template/button-has-type": "error",
        "@angular-eslint/template/banana-in-box": "error",
        "@angular-eslint/template/no-duplicate-attributes": "error",
        "@angular-eslint/template/label-has-associated-control": "error",
        "@angular-eslint/template/conditional-complexity": [
            "warn",
            {
                "maxComplexity": 15
            }
        ],
        // "@angular-eslint/template/cyclomatic-complexity": [
        //     "warn",
        //     {
        //         "maxComplexity": 10
        //     }
        // ],
        "@angular-eslint/template/no-distracting-elements": "error",
        "@angular-eslint/template/no-duplicate-attributes": "error",
        "@angular-eslint/template/no-inline-styles": [
            "warn",
            {
                "allowNgStyle": true,
                "allowBindToStyle": true
            }
        ],
        "@angular-eslint/template/valid-aria": "error",
        "@angular-eslint/template/no-positive-tabindex": "error",
        "@angular-eslint/template/no-negated-async": "error",
        "@angular-eslint/template/table-scope": "error",
        "@angular-eslint/template/label-has-associated-control": "error",
    },
}];