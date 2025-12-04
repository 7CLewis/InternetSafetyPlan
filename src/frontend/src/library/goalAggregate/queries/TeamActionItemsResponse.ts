export class TeamActionItemsResponse {
  public id: string;
  public goalId: string;
  public goalName: string;
  public name: string;
  public description: string | null;
  public dueDate: string | null;
  public isComplete: boolean;

  constructor(id: string, goalId: string, goalName: string, name: string, dueDate: string | null, isComplete: boolean, description: string | null) {
    this.id = id;
    this.goalId = goalId;
    this.goalName = goalName;
    this.name = name;
    this.description = description;
    this.dueDate = dueDate;
    this.isComplete = isComplete;
  }
}