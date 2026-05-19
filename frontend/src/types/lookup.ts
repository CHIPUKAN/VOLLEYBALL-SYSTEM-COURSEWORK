// справочник с числовым кодом (S1–S18)
export interface LookupDto {
  code: number;
  name: string;
}

// справочник с строковым id (регионы, тренеры, площадки и т.д.)
export interface LookupItemDto {
  id: string;
  name: string;
}
