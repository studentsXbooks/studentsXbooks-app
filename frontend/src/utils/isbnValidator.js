const isbn10ModCheck = 11;
const isbn13ModCheck = 10;
const isbn10DigitLength = 10;
const isbn13DigitLength = 13;
const sum = (a, b) => a + b;
const multiply = (a, b) => a * b;
const xValue = 10;
const radixValue = 10;

const removeDashes = input =>
  input
    .trim()
    .split("-")
    .join("");

const convertToInt = char => Number.parseInt(char, radixValue);

const isISBN10 = (possibleISBN: string) => {
  const cleanedString = removeDashes(possibleISBN);

  if (cleanedString.length !== isbn10DigitLength) return false;

  return (
    [...cleanedString]
      .map(char => {
        if (char === "X" || char === "x") {
          return xValue;
        }
        return convertToInt(char);
      })
      .map((value, index) => multiply(value, index + 1))
      .reduce(sum) %
      isbn10ModCheck ===
    0
  );
};

// ISBN13 multiples either by three or one depending on index location
const isbn13UseThreeOrOne = index => (index % 2 === 1 ? 3 : 1);

const isISBN13 = (possibleISBN: string) => {
  const cleanedString = removeDashes(possibleISBN);

  if (cleanedString.length !== isbn13DigitLength) return false;

  return (
    [...cleanedString]
      .map((char, index) => [convertToInt(char), isbn13UseThreeOrOne(index)])
      .map(([value, index]) => multiply(value, index))
      .reduce(sum) %
      isbn13ModCheck ===
    0
  );
};

export { isISBN10, isISBN13 };
