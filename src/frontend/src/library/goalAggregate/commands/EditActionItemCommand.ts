export class EditActionItemCommand {
  public goalId: string;
  public actionItemId: string;
  public name: string;
  public description: string | null;
  public dueDate: string | null;

  constructor(goalId: string, actionItemId: string, name: string, description: string | null, dueDate: string | null) {
    this.goalId = goalId;
    this.actionItemId = actionItemId;
    this.name = name;
    this.description = description;
    this.dueDate = dueDate;
  }
}