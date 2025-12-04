export class CreateEditGoalAndActionItems {
  public id: string;
  public ultimateGoalId: string;
  public name: string;
  public category: number;
  public description: string | null;
  public dueDate: string | null;
  public actionItems: CreateEditGoalAndActionItems_ActionItem[];

  constructor(id: string, ultimateGoalId: string, name: string, category: number, description: string | null, dueDate: string | null, actionItems: CreateEditGoalAndActionItems_ActionItem[]) {
    this.id = id;
    this.ultimateGoalId = ultimateGoalId;
    this.name = name;
    this.category = category;
    this.description = description;
    this.dueDate = dueDate;
    this.actionItems = actionItems;
  }
}

export class CreateEditGoalAndActionItems_ActionItem {
  public id: string;
  public name: string;
  public description: string | null;
  public dueDate: string;

  constructor(id: string, name: string, description: string | null, dueDate: string) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.dueDate = dueDate;
  }
}