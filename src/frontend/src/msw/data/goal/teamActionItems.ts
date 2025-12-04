import { ApiResponse } from 'utils/redux/apiResponse';
import { actionItem1Id, actionItem2Id, actionItem3Id, actionItem4Id, actionItem5Id, actionItem6Id, actionItem7Id, actionItem8Id, actionItem9Id, goal1Id, goal2Id, goal3Id } from 'msw/data/shared/sharedVariables';
import { TeamActionItemsResponse } from 'library/goalAggregate/queries/TeamActionItemsResponse';

export const TeamActionItems: ApiResponse<TeamActionItemsResponse[]> = {
  error: {
    code: '',
    message: ''
  },
  isFailure: false,
  isSuccess: true,
  value: [
    {
      id: actionItem1Id,
      goalId: goal1Id,
      goalName: 'Establish a new device-free tradition',
      name: 'Read blog article about internet safety',
      description: '',
      dueDate: new Date(2024,12,12).toISOString(),
      isComplete: false
    },
    {
      id: actionItem2Id,
      goalId: goal1Id,
      goalName: 'Establish a new device-free tradition',
      name: 'Try out one new device-free activity',
      description: '',
      dueDate: new Date(2023,11,13).toISOString(),
      isComplete: true
    },
    {
      id: actionItem3Id,
      goalId: goal1Id,
      goalName: 'Establish a new device-free tradition',
      name: 'Try out another new device-free activity',
      description: '',
      dueDate: new Date(2023,8,12).toISOString(),
      isComplete: true
    },
    {
      id: actionItem4Id,
      goalId: goal1Id,
      goalName: 'Establish a new device-free tradition',
      name: 'Decide on the tradition and implement a schedule',
      description: '',
      dueDate: null,
      isComplete: true
    },
    {
      id: actionItem5Id,
      goalId: goal2Id,
      goalName: 'Research and purchase a router with good parental controls',
      name: 'Research Gryphon Router Features',
      description: 'https://gryphonconnect.com',
      dueDate: new Date(2023,9,3).toISOString(),
      isComplete: false
    },
    {
      id: actionItem6Id,
      goalId: goal2Id,
      goalName: 'Research and purchase a router with good parental controls',
      name: 'Research Firewalla Router Features',
      description: 'https://firewalla.com',
      dueDate: new Date(2023,8,4).toISOString(),
      isComplete: true
    },
    {
      id: actionItem7Id,
      goalId: goal3Id,
      goalName: 'Talk with kids about internet safety',
      name: 'Talk with Noah about internet safety topics',
      description: '',
      dueDate: null,
      isComplete: false
    },
    {
      id: actionItem8Id,
      goalId: goal3Id,
      goalName: 'Talk with kids about internet safety',
      name: 'Talk with Anna about internet safety topics',
      description: '',
      dueDate: new Date(2023,8,3).toISOString(),
      isComplete: false
    },
    {
      id: actionItem9Id,
      goalId: goal3Id,
      goalName: 'Talk with kids about internet safety',
      name: 'Talk with Ronan about internet safety topics',
      description: '',
      dueDate: new Date().toISOString(),
      isComplete: true
    }
  ]
};