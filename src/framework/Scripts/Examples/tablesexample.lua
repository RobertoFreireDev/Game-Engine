local tablesexample={
    title = "Table example"
}

systemlogsTable = {}

function printTable(tbl, indent, x, y)
    indent = indent or 0
    x = x or 10
    y = y or 10
    local indentStr = string.rep("  ", indent)
    for k, v in pairs(tbl) do
        if type(v) == "table" then
            _print(indentStr .. k .. ":", x, y, 1)
            y = y + 10  -- move down for next line
            y = printTable(v, indent + 1, x, y)
        else
            _print(indentStr .. k .. ": " .. tostring(v), x, y, 1)
            y = y + 10
        end
    end
    return y
end

function tablesexample:init()
    systemlogsTable = _tbra("systemlogs")
end

function tablesexample:update() 
end

function tablesexample:draw()
    printTable(systemlogsTable)
end

return tablesexample