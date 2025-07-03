export class ColorUtils {

  public static isDarkColor(color: string): boolean {
    if (!color || color.trim() === '') return false;

    try {
      const colorValues = color.replace(/^(rgb|rgba)\(/, '').replace(/\)$/, '').split(',');

      if (colorValues.length < 3) return false;

      const r = parseInt(colorValues[0]?.trim() ?? '0', 10) || 0;
      const g = parseInt(colorValues[1]?.trim() || '0', 10) || 0;
      const b = parseInt(colorValues[2]?.trim() || '0', 10) || 0;

      const validR = Math.min(Math.max(r, 0), 255);
      const validG = Math.min(Math.max(g, 0), 255);
      const validB = Math.min(Math.max(b, 0), 255);

      // Calculate luminance using YIQ formula
      const yiq = ((validR * 299) + (validG * 587) + (validB * 114)) / 1000;

      // YIQ values < 150 are considered dark colors
      return yiq < 150;
    } catch (error) {
      return false;
    }
  }

  public static hexCodeToRgbValue(hex: string): { r: number; g: number; b: number } | null {
    const match = hex.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);

    if (!match) {
      return null;
    }

    return {
      r: parseInt(match[1] ?? '0', 10),
      g: parseInt(match[2] ?? '0', 10),
      b: parseInt(match[3] ?? '0', 10),
    };
  }

  public static rgbValueToHexCode(r: number, g: number, b: number): string {
    const toHex = (color: number): string => {
      const hex = color.toString(16);
      return hex.length === 1 ? '0' + hex : hex;
    };

    return `#${toHex(r)}${toHex(g)}${toHex(b)}`;
  }

  public static rgbValueToRgbString(r: number, g: number, b: number): string {
    return `rgb(${r}, ${g}, ${b})`;
  }

}