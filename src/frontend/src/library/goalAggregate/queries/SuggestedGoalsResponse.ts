export class SuggestedGoalsResponse {
  public id: string;
  public ultimateGoalId: string;
  public name: string;
  public description: string | null;
  public dueDate: string | null;
  public isComplete: boolean;
  public actionItems: SuggestedGoalsResponse_ActionItem[] | null;
  public category: number;

  constructor(id: string, ultimateGoalId: string, name: string, description: string, dueDate: string | null, actionItems: SuggestedGoalsResponse_ActionItem[] | null, isComplete: boolean, category: number) {
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

export class SuggestedGoalsResponse_ActionItem {
  public id: string;
  public name: string;
  public description: string | null;
  public dueDate: string | null;
  public isComplete: boolean;

  constructor(id: string, name: string, description: string, dueDate: string | null, isComplete: boolean) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.dueDate = dueDate;
    this.isComplete = isComplete;
  }
}