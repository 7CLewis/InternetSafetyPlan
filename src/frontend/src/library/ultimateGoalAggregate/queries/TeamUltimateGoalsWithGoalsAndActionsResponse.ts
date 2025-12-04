export class UltimateGoalsWithGoalsAndActionsResponse {
  public id: string;
  public name: string;
  public description: string | null;
  public goals: UltimateGoalsWithGoalsAndActionsResponse_Goal[] | null;

  constructor(id: string, name: string, description: string | null, goals: UltimateGoalsWithGoalsAndActionsResponse_Goal[] | null) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.goals = goals;
  }
}

export class UltimateGoalsWithGoalsAndActionsResponse_Goal {
  public id: string;
  public name: string;
  public actions: UltimateGoalsWithGoalsAndActionsResponse_Goal_Action[] | null;

  constructor(id: string, name: string, actions: UltimateGoalsWithGoalsAndActionsResponse_Goal_Action[] | null) {
    this.id = id;
    this.name = name;
    this.actions = actions;
  }
}

export class UltimateGoalsWithGoalsAndActionsResponse_Goal_Action {
  public id: string;
  public name: string;
  public description: string | null;
  public dueDate: string | null;
  public isComplete: boolean;

  constructor(id: string, name: string, description: string | null, dueDate: string | null, isComplete: boolean) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.dueDate = dueDate;
    this.isComplete = isComplete;
  }
}