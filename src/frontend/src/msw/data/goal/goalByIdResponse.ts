import { ApiResponse } from 'utils/redux/apiResponse';
import { actionItem1Id, actionItem2Id, actionItem3Id, actionItem4Id, actionItem5Id, actionItem6Id, actionItem7Id, actionItem8Id, actionItem9Id, goal1Id, goal2Id, goal3Id } from 'msw/data/shared/sharedVariables';
import { TeamGoalsResponse } from 'library/goalAggregate/queries/TeamGoalsResponse';

export const GoalsById: ApiResponse<TeamGoalsResponse>[] = [{
  error: {
    code: '',
    message: ''
  },
  isFailure: false,
  isSuccess: true,
  value:
    {
      id: goal1Id,
      name: 'Establish a new device-free tradition',
      description: 'We want to make sure we have a good family event that is fun for everyone and doesn\'t involve any internet-connected devices.',
      isComplete: false,
      dueDate: new Date().toISOString(),
      actionItems: [{
        id: actionItem1Id,
        name: 'Read blog article about internet safety',
        description: 'https://internetsafetyconsulting.com/how-to-raise-internet-safe-children/',
        dueDate: new Date().toISOString(),
        isComplete: false
      },
      {
        id: actionItem2Id,
        name: 'Try out one new device-free activity',
        description: 'Based on article and discussions with family, try a device-free activity with the whole family.',
        dueDate: new Date().toISOString(),
        isComplete: true
      },
      {
        id: actionItem3Id,
        name: 'Try out another new device-free activity',
        description: 'Based on article and discussions with family, try a device-free activity with the whole family.',
        dueDate: new Date().toISOString(),
        isComplete: true
      },
      {
        id: actionItem4Id,
        name: 'Decide on the tradition and implement a schedule',
        description: 'Discuss with the family which idea(s) was/were their favorite, and create a schedule to repeatedly do them.',
        dueDate: new Date().toISOString(),
        isComplete: true
      }]
    }
  },
  {
    error: {
      code: '',
      message: ''
    },
    isFailure: false,
    isSuccess: true,
    value: {
      id: goal2Id,
      name: 'Research and purchase a router with good parental controls',
      description: 'Look into Gryphon, Firewalla, and other routers and find the best one for our family',
      isComplete: false,
      dueDate: new Date().toISOString(),
      actionItems: [{
        id: actionItem5Id,
        name: 'Research Gryphon Router Features',
        description: 'https://gryphonconnect.com',
        dueDate: new Date().toISOString(),
        isComplete: false
      },
      {
        id: actionItem6Id,
        name: 'Research Firewalla Router Features',
        description: 'https://firewalla.com',
        dueDate: new Date().toISOString(),
        isComplete: true
      }]
    }
  },
  {
    error: {
      code: '',
      message: ''
    },
    isFailure: false,
    isSuccess: true,
    value: {
      id: goal3Id,
      name: 'Talk with kids about internet safety',
      description: 'Have an open discussion with each kid individually about whatever questions/experiences they\'ve had with technology recently.',
      isComplete: false,
      dueDate: new Date().toISOString(),
      actionItems: [{
        id: actionItem7Id,
        name: 'Talk with Noah about internet safety topics',
        description: 'Check in on his interactions with technology at school and friend\'s house',
        dueDate: new Date().toISOString(),
        isComplete: false
      },
      {
        id: actionItem8Id,
        name: 'Talk with Anna about internet safety topics',
        description: 'Read \'Good Pictures, Bad Pictures\' with her.',
        dueDate: new Date().toISOString(),
        isComplete: false
      },
      {
        id: actionItem9Id,
        name: 'Talk with Ronan about internet safety topics',
        description: 'Talk about phone usage, recent search histories, etc.',
        dueDate: new Date().toISOString(),
        isComplete: true
      }]
    }
  },
  {
    error: {
      code: '',
      message: ''
    },
    isFailure: false,
    isSuccess: true,
    value: null
  }
];