import { ActionItem } from 'library/goalAggregate/ActionItem';

export class Goal {
  public id: string;
  public ultimateGoalId: string;
  public name: string;
  public description: string | null;
  public dueDate: string | null;
  public isComplete: boolean;
  public actionItems: ActionItem[] | null;
  public category: number;

  constructor(id: string, ultimateGoalId: string, name: string, description: string, dueDate: string | null, actionItems: ActionItem[] | null, isComplete: boolean, category: number) {
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