import { ApiResponse } from 'utils/redux/apiResponse';
import { actionItem1Id, actionItem2Id, actionItem3Id, actionItem4Id, actionItem5Id, actionItem6Id, goal1Id, goal2Id, goal3Id, goal4Id, ultimateGoal1Id, ultimateGoal2Id, ultimateGoal3Id } from 'msw/data/shared/sharedVariables';
import { UltimateGoalsWithGoalsAndActionsResponse } from 'library/ultimateGoalAggregate/queries/TeamUltimateGoalsWithGoalsAndActionsResponse';

export const TeamUltimateGoalsWithGoalsAndActions: ApiResponse<UltimateGoalsWithGoalsAndActionsResponse[]> = {
  error: {
    code: '',
    message: ''
  },
  isFailure: false,
  isSuccess: true,
  value: [
  {
    id: ultimateGoal1Id,
    name: 'Foster an open & grace-filled environment surrounding technology',
    description: 'Work to have kids feel comfortable discussing anything about technology - the good and the bad',
    goals: [
      {
        id: goal2Id,
        name: 'Talk with kids about internet safety',
        actions: [{
            id: actionItem4Id,
            name: 'Talk with Ronan about internet safety',
            description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas tincidunt velit in eros bibendum tincidunt. Curabitur vulputate mauris sed libero sodales suscipit. Phasellus lobortis semper lacus sit amet vulputate.',
            dueDate: new Date(2024,7,12).toISOString(),
            isComplete: false
          }
        ]
      },
      {
        id: goal3Id,
        name: 'Demonstrate healthy technology usage',
        actions: [{
            id: actionItem5Id,
            name: 'Have video game night with kids',
            description: '',
            dueDate: new Date(2024,2,15).toISOString(),
            isComplete: false
          }
        ]
      }
    ]
  },
  {
    id: ultimateGoal2Id,
    name: 'Maintain an internet-safe home',
    description: 'Enforce and improve upon internet safety practices within our home.',
    goals: [
      {
        id: goal1Id,
        name: 'Research router options',
        actions: [{
            id: actionItem1Id,
            name: 'Read PYE article about routers',
            description: '',
            dueDate: new Date(2024,1,17).toISOString(),
            isComplete: false
          },
          {
            id: actionItem2Id,
            name: 'Purchase best-fit option',
            description: '',
            dueDate: new Date(2024,2,23).toISOString(),
            isComplete: false
          },
          {
            id: actionItem3Id,
            name: 'Set up new router and profiles',
            description: '',
            dueDate: new Date(2024,3,1).toISOString(),
            isComplete: false
          }
        ]
      }
    ]
  },
  {
    id: ultimateGoal3Id,
    name: 'Share internet safety practices with those around me',
    description: 'Share all of my learnings about internet safety with those in my community, and encourage them in their internet safety journey.',
    goals: [
      {
        id: goal4Id,
        name: 'Post about internet safety on social media',
        actions: [{
            id: actionItem6Id,
            name: 'Share game night ideas on Facebook',
            description: '',
            dueDate: new Date(2024,5,2).toISOString(),
            isComplete: false
          }
        ]
      }
    ]
  }]
};