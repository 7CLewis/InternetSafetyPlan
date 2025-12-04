export class ActionItem {
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