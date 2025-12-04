export class TeamGoalsResponse {
  public id: string;
  public ultimateGoalId: string;
  public name: string;
  public description: string | null;
  public dueDate: string;
  public isComplete: boolean;
  public actionItems: TeamGoalsResponse_ActionItem[] | null;
  public category: number;

  constructor(id: string, ultimateGoalId: string, name: string, description: string, dueDate: string, actionItems: TeamGoalsResponse_ActionItem[] | null, isComplete: boolean, category: number) {
    this.id = id;
    this.ultimateGoalId = ultimateGoalId;
    this.name = name;
    this.description = description;
    this.dueDate = dueDate;
    this.actionItems = actionItems;
    this.isComplete = isComplete;
    this.category = category;
  }
}

export class TeamGoalsResponse_ActionItem {
  public id: string;
  public name: string;
  public description: string | null;
  public dueDate: string;
  public isComplete: boolean;

  constructor(id: string, name: string, description: string, dueDate: string, isComplete: boolean) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.dueDate = dueDate;
    this.isComplete = isComplete;
  }
}