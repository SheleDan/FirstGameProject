const sharp = require('../../.image-tools/node_modules/sharp');

const [input, output] = process.argv.slice(2);
if (!input || !output) {
  throw new Error('Usage: node remove-chroma.js <input.png> <output.png>');
}

(async () => {
  const { data, info } = await sharp(input).ensureAlpha().raw().toBuffer({ resolveWithObject: true });
  for (let i = 0; i < data.length; i += 4) {
    const r = data[i], g = data[i + 1], b = data[i + 2];
    const distance = Math.sqrt(r * r + (255 - g) ** 2 + b * b);
    const alpha = Math.max(0, Math.min(255, Math.round((distance - 48) / (180 - 48) * 255)));
    data[i + 3] = Math.min(data[i + 3], alpha);

    // Remove green spill only from partially transparent edge pixels.
    if (alpha < 250) {
      const neutralGreen = Math.max(r, b) + 18;
      data[i + 1] = Math.min(g, neutralGreen);
    }
  }

  await sharp(data, { raw: info }).png().toFile(output);
  console.log(output);
})();
