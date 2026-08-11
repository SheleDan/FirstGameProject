const opentype = require('../../.font-tools/node_modules/opentype.js');
const fs = require('fs');
const path = require('path');

// Riftbound Pixel Regular — an original compact 5x7 pixel face.
// Glyphs are deliberately built from integer cells so they remain crisp in TMP.
const rows = (...value) => value;
const glyphs = {
  ' ': rows('00000','00000','00000','00000','00000','00000','00000'),
  '!': rows('00100','00100','00100','00100','00100','00000','00100'),
  '?': rows('01110','10001','00001','00010','00100','00000','00100'),
  '.': rows('00000','00000','00000','00000','00000','00110','00110'),
  ',': rows('00000','00000','00000','00000','00110','00110','00100'),
  ':': rows('00000','00110','00110','00000','00110','00110','00000'),
  ';': rows('00000','00110','00110','00000','00110','00110','00100'),
  '-': rows('00000','00000','00000','11111','00000','00000','00000'),
  '_': rows('00000','00000','00000','00000','00000','00000','11111'),
  '+': rows('00000','00100','00100','11111','00100','00100','00000'),
  '=': rows('00000','00000','11111','00000','11111','00000','00000'),
  '/': rows('00001','00010','00010','00100','01000','01000','10000'),
  '\\':rows('10000','01000','01000','00100','00010','00010','00001'),
  '(': rows('00010','00100','01000','01000','01000','00100','00010'),
  ')': rows('01000','00100','00010','00010','00010','00100','01000'),
  '[': rows('01110','01000','01000','01000','01000','01000','01110'),
  ']': rows('01110','00010','00010','00010','00010','00010','01110'),
  '"':rows('01010','01010','01010','00000','00000','00000','00000'),
  "'":rows('00100','00100','00010','00000','00000','00000','00000'),
  '%': rows('11001','11010','00100','00100','01000','10110','00110'),
  '#': rows('01010','11111','01010','01010','11111','01010','00000'),
  '0': rows('01110','10001','10011','10101','11001','10001','01110'),
  '1': rows('00100','01100','00100','00100','00100','00100','01110'),
  '2': rows('01110','10001','00001','00010','00100','01000','11111'),
  '3': rows('11110','00001','00001','01110','00001','00001','11110'),
  '4': rows('00010','00110','01010','10010','11111','00010','00010'),
  '5': rows('11111','10000','10000','11110','00001','00001','11110'),
  '6': rows('01110','10000','10000','11110','10001','10001','01110'),
  '7': rows('11111','00001','00010','00100','01000','01000','01000'),
  '8': rows('01110','10001','10001','01110','10001','10001','01110'),
  '9': rows('01110','10001','10001','01111','00001','00001','01110'),
  A: rows('01110','10001','10001','11111','10001','10001','10001'),
  B: rows('11110','10001','10001','11110','10001','10001','11110'),
  C: rows('01111','10000','10000','10000','10000','10000','01111'),
  D: rows('11110','10001','10001','10001','10001','10001','11110'),
  E: rows('11111','10000','10000','11110','10000','10000','11111'),
  F: rows('11111','10000','10000','11110','10000','10000','10000'),
  G: rows('01111','10000','10000','10111','10001','10001','01110'),
  H: rows('10001','10001','10001','11111','10001','10001','10001'),
  I: rows('01110','00100','00100','00100','00100','00100','01110'),
  J: rows('00111','00010','00010','00010','10010','10010','01100'),
  K: rows('10001','10010','10100','11000','10100','10010','10001'),
  L: rows('10000','10000','10000','10000','10000','10000','11111'),
  M: rows('10001','11011','10101','10101','10001','10001','10001'),
  N: rows('10001','11001','10101','10011','10001','10001','10001'),
  O: rows('01110','10001','10001','10001','10001','10001','01110'),
  P: rows('11110','10001','10001','11110','10000','10000','10000'),
  Q: rows('01110','10001','10001','10001','10101','10010','01101'),
  R: rows('11110','10001','10001','11110','10100','10010','10001'),
  S: rows('01111','10000','10000','01110','00001','00001','11110'),
  T: rows('11111','00100','00100','00100','00100','00100','00100'),
  U: rows('10001','10001','10001','10001','10001','10001','01110'),
  V: rows('10001','10001','10001','10001','10001','01010','00100'),
  W: rows('10001','10001','10001','10101','10101','10101','01010'),
  X: rows('10001','10001','01010','00100','01010','10001','10001'),
  Y: rows('10001','10001','01010','00100','00100','00100','00100'),
  Z: rows('11111','00001','00010','00100','01000','10000','11111'),
};

const cyr = {
  'А':'A','Б':rows('11111','10000','10000','11110','10001','10001','11110'),
  'В':'B','Г':rows('11111','10000','10000','10000','10000','10000','10000'),
  'Д':rows('00110','01010','01010','01010','01010','11111','10001'),
  'Е':'E','Ё':rows('01010','00000','11111','10000','11110','10000','11111'),
  'Ж':rows('10101','10101','01110','00100','01110','10101','10101'),
  'З':rows('11110','00001','00001','01110','00001','00001','11110'),
  'И':rows('10001','10001','10011','10101','11001','10001','10001'),
  'Й':rows('01010','00100','10001','10011','10101','11001','10001'),
  'К':'K','Л':rows('00111','01001','01001','01001','01001','01001','10001'),
  'М':'M','Н':'H','О':'O','П':rows('11111','10001','10001','10001','10001','10001','10001'),
  'Р':'P','С':'C','Т':'T','У':rows('10001','10001','10001','01111','00001','00001','11110'),
  'Ф':rows('00100','01110','10101','10101','01110','00100','00100'),
  'Х':'X','Ц':rows('10010','10010','10010','10010','10010','11111','00001'),
  'Ч':rows('10001','10001','10001','01111','00001','00001','00001'),
  'Ш':rows('10101','10101','10101','10101','10101','10101','11111'),
  'Щ':rows('10100','10100','10100','10100','10100','11111','00001'),
  'Ъ':rows('11000','01000','01000','01110','01001','01001','01110'),
  'Ы':rows('10001','10001','10001','11101','10011','10011','11101'),
  'Ь':rows('10000','10000','10000','11110','10001','10001','11110'),
  'Э':rows('11110','00001','00001','01111','00001','00001','11110'),
  'Ю':rows('10010','10101','10101','11101','10101','10101','10010'),
  'Я':rows('01111','10001','10001','01111','00101','01001','10001'),
};

for (const [letter, shape] of Object.entries(cyr)) glyphs[letter] = typeof shape === 'string' ? glyphs[shape] : shape;
for (const letter of 'ABCDEFGHIJKLMNOPQRSTUVWXYZ') glyphs[letter.toLowerCase()] = glyphs[letter];
for (const letter of 'АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ') glyphs[letter.toLowerCase()] = glyphs[letter];

function makeGlyph(char, bitmap) {
  const p = new opentype.Path();
  const isLower = char.toLowerCase() === char && char.toUpperCase() !== char;
  const hasDescender = 'gjpqyруфцщ'.includes(char);
  const cellX = isLower ? 82 : 96;
  const cellY = isLower ? 76 : 96;
  const usedColumns = [];
  for (let x = 0; x < bitmap[0].length; x++) {
    if (bitmap.some(row => row[x] === '1')) usedColumns.push(x);
  }
  const left = usedColumns.length ? usedColumns[0] : 0;
  const right = usedColumns.length ? usedColumns[usedColumns.length - 1] : 3;
  const baselineShift = hasDescender ? -2 * cellY : 0;
  const filled = (x, y) => y >= 0 && y < bitmap.length &&
    x >= 0 && x < bitmap[y].length && bitmap[y][x] === '1';
  const edges = [];
  const addEdge = (ax, ay, bx, by) => edges.push({ a: [ax, ay], b: [bx, by], used: false });

  // Add only exposed cell edges. Stitching these edges below creates one
  // continuous outline instead of stacked rectangles with visible seams.
  for (let y = 0; y < bitmap.length; y++) {
    for (let x = 0; x < bitmap[y].length; x++) {
      if (!filled(x, y)) continue;
      if (!filled(x, y + 1)) addEdge(x, y + 1, x + 1, y + 1); // bottom
      if (!filled(x + 1, y)) addEdge(x + 1, y + 1, x + 1, y); // right
      if (!filled(x, y - 1)) addEdge(x + 1, y, x, y);         // top
      if (!filled(x - 1, y)) addEdge(x, y, x, y + 1);         // left
    }
  }

  const pointKey = v => `${v[0]},${v[1]}`;
  const outgoing = new Map();
  edges.forEach((edge, index) => {
    const key = pointKey(edge.a);
    if (!outgoing.has(key)) outgoing.set(key, []);
    outgoing.get(key).push(index);
  });

  const seed = char.codePointAt(0);
  const transform = ([gx, gy]) => {
    const xWobble = (((gy * 17 + seed * 7) % 7) - 3) * 1.15;
    const yWobble = (((gx * 11 + seed * 3) % 5) - 2) * 0.75;
    return [
      (gx - left) * cellX + xWobble,
      (bitmap.length - gy) * cellY + baselineShift + yWobble
    ];
  };

  for (let startIndex = 0; startIndex < edges.length; startIndex++) {
    if (edges[startIndex].used) continue;
    const loop = [];
    let current = startIndex;
    const startKey = pointKey(edges[current].a);
    let guard = 0;
    while (!edges[current].used && guard++ < edges.length + 2) {
      const edge = edges[current];
      edge.used = true;
      loop.push(edge.a);
      const nextKey = pointKey(edge.b);
      if (nextKey === startKey) break;
      const choices = (outgoing.get(nextKey) || []).filter(i => !edges[i].used);
      if (!choices.length) break;
      current = choices[0];
    }
    if (loop.length < 3) continue;

    const points = loop.map(transform);
    const radius = isLower ? 7 : 8;
    const corner = (prev, v, next) => {
      const toward = (from, to) => {
        const dx = to[0] - from[0], dy = to[1] - from[1];
        const len = Math.hypot(dx, dy) || 1;
        return [from[0] + dx / len * radius, from[1] + dy / len * radius];
      };
      return { before: toward(v, prev), after: toward(v, next) };
    };
    const first = corner(points.at(-1), points[0], points[1]);
    p.moveTo(first.after[0], first.after[1]);
    for (let i = 1; i <= points.length; i++) {
      const index = i % points.length;
      const c = corner(points[(index - 1 + points.length) % points.length], points[index], points[(index + 1) % points.length]);
      p.lineTo(c.before[0], c.before[1]);
      p.quadTo(points[index][0], points[index][1], c.after[0], c.after[1]);
    }
    p.close();
  }
  return new opentype.Glyph({
    name: char === ' ' ? 'space' : `uni${char.codePointAt(0).toString(16).toUpperCase().padStart(4,'0')}`,
    unicode: char.codePointAt(0),
    advanceWidth: char === ' ' ? 320 : (right - left + 1) * cellX + 78,
    path: p
  });
}

const notdef = makeGlyph('\uFFFD', rows('11111','10001','10101','10101','10101','10001','11111'));
const outputGlyphs = [notdef, ...Object.entries(glyphs).map(([c,b]) => makeGlyph(c,b))];
const font = new opentype.Font({
  familyName: 'Riftbound Hand Pixel', styleName: 'Regular',
  designer: 'FirstGameProject', manufacturer: 'FirstGameProject',
  description: 'Original soft hand-drawn pixel typeface for FirstGameProject.',
  unitsPerEm: 1000, ascender: 800, descender: -200, glyphs: outputGlyphs
});

const outDir = path.resolve(__dirname, '../../Assets/Game/Art/FirstGameProject/Generated/Fonts');
fs.mkdirSync(outDir, { recursive: true });
const outFile = path.join(outDir, 'RiftboundHandPixel-Regular.ttf');
fs.writeFileSync(outFile, Buffer.from(font.toArrayBuffer()));
console.log(`${outFile}\nGlyphs: ${outputGlyphs.length}`);
