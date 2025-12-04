export class ToggleActionItemCompletionCommand {
  public goalId: string;
  public actionItemId: string;

  constructor(goalId: string, actionItemId: string) { this.goalId = goalId; this.actionItemId = actionItemId; }
}