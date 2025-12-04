// src/mocks/browser.js
import { setupWorker } from 'msw';
import mockUserHandlers from 'msw/mockUserHandler';
import mockTeamHandlers from 'msw/mockTeamHandler';
import mockUltimateGoalHandlers from 'msw/mockUltimateGoalHandler';
import mockGoalHandlers from 'msw/mockGoalHandler';
import mockDeviceHandlers from 'msw/mockDeviceHandler';

// This configures a Service Worker with the given request handlers.
const handlers = [...mockDeviceHandlers, ...mockGoalHandlers, ...mockTeamHandlers, ...mockUltimateGoalHandlers, ...mockUserHandlers];
export const worker = setupWorker(...handlers);