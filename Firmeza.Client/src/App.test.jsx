import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import App from './App';

describe('App', () => {
    it('renders without crashing', () => {
        render(<App />);
        // Add specific assertions based on App content
        // For now, just checking if it renders is a good start
        expect(document.body).toBeDefined();
    });
});
