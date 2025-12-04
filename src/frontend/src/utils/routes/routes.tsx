import React from 'react';
import { Route, Routes } from 'react-router-dom';
import { RequireAuth } from 'utils/auth/auth';
import { Layout } from 'components/base';
import Login from 'pages/login';
import Main from 'pages/main';
import NoMatch from 'pages/no-match';
import InitialSetup from 'pages/initial-setup';
import UltimateGoals from 'pages/settings/ultimate-goals';
import Goals from 'pages/goals';
import Actions from 'pages/actions';
import Team from 'pages/team';
import Devices from 'pages/devices';
import Goal from 'pages/goals/goal';

export default function AppRoutes(): JSX.Element {
  return (
    <Routes>
      <Route element={ <Layout /> }>
        <Route element={ <RequireAuth /> }>
          <Route path='' element={ <Main /> } />
          <Route path='/actions' element={ <Actions /> } />
          <Route path='/devices' element={ <Devices /> } />
          <Route path='/goals' element={ <Goals /> } />
          <Route path='/goals/:goalId' element={ <Goal /> } />
          <Route path='/initial-setup' element={ <InitialSetup /> } />
          <Route path='/login' element={ <Login /> } />
          <Route path='/settings/team' element={ <UltimateGoals /> } />
          <Route path='/settings/ultimate-goals' element={ <UltimateGoals /> } />
          <Route path='/team' element={ <Team /> } />
          <Route path='*' element={ <NoMatch /> } />
        </Route>
      </Route>
    </Routes>
  );
}