export class UltimateGoal {
  public id: string;
  public name: string;
  public description: string | null;

  constructor(id: string, name: string, description: string) {
    this.id = id;
    this.name = name;
    this.description = description;
  }
}