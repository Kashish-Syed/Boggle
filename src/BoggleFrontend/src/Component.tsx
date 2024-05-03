import axios from "axios";

const validateInput = async (userInput: string): Promise<boolean> => {
  userInput = userInput.toLowerCase();
  try {
    const response = await axios.post(
      "http://localhost:5189/api/Boggle/isValidWord",
      JSON.stringify(userInput),
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    );
    await new Promise((resolve) => setTimeout(resolve, 1000));
    return response.data;
  } catch (error) {
    console.error("Error validating input:", error + " " + userInput);
    return false;
  }
};

export default validateInput;
