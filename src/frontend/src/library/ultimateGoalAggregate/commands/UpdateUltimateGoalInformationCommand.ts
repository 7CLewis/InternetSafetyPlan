export class UpdateUltimateGoalInformationCommand {
  public ultimateGoalId: string;
  public name: string;
  public description: string | null;

  constructor(ultimateGoalId: string, name: string, description: string) {
    this.ultimateGoalId = ultimateGoalId;
    this.name = name;
    this.description = description;
  }
}