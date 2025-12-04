/* eslint-env node */
module.exports = {
  extends: [
    'eslint:recommended',
    'plugin:@typescript-eslint/recommended',
    'plugin:@typescript-eslint/recommended-requiring-type-checking',
  ],
  plugins: ['@typescript-eslint', 'react'],
  parser: '@typescript-eslint/parser',
  parserOptions: {
    project: ['./tsconfig.json'],
    tsconfigRootDir: __dirname,
  },
  root: true,
  rules: {
    'no-trailing-spaces': 'error',
    'semi': [2, 'always'],
    // Note: you must disable the base rule as it can report incorrect errors
    'comma-dangle': 'off',
    '@typescript-eslint/comma-dangle': 'error',
    'no-restricted-imports': ['error', {
      'patterns': ['.*']
      }
    ],
    'react/jsx-curly-spacing': ['error', {
      'when': 'always', 
      'children': true
    }],
    'quotes': ['error', 'single'],
    'jsx-quotes': ['error', 'prefer-single']
  }
};