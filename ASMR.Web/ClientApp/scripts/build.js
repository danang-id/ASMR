const fs = require("fs")
const path = require("path")

const now = new Date()
const monthDigitList = "ABCDEFGHIJKL"
const dateDigitList = "abcdefghijklmnopqrstuvwxyzABCDE"
const yearDigit = now.getFullYear().toString().substring(2)
const monthDigit = monthDigitList.charAt(now.getMonth())
const dateDigit = dateDigitList.charAt(now.getDate() - 1)
const timeDigit = now.getHours() >= 10 ? now.getHours().toString() : `0${now.getHours()}`

const build = {
	number: yearDigit + monthDigit + dateDigit + timeDigit
}
const buildFilePath = path.join(__dirname, "..", "src", "build.json")
fs.writeFileSync(buildFilePath, JSON.stringify(build, null, 4))
