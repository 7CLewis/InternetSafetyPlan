export class CreateGoalAndActionItemsCommand {
  public ultimateGoalId: string;
  public name: string;
  public category: number;
  public description: string | null;
  public dueDate: string | null;
  public actionItems: CreateGoalAndActionItemsCommand_ActionItem[];

  constructor(ultimateGoalId: string, name: string, category: number, description: string | null, dueDate: string | null, actionItems: CreateGoalAndActionItemsCommand_ActionItem[]) {
    this.ultimateGoalId = ultimateGoalId;
    this.name = name;
    this.category = category;
    this.description = description;
    this.dueDate = dueDate;
    this.actionItems = actionItems;
  }
}

export class CreateGoalAndActionItemsCommand_ActionItem {
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