-- Table functions

function add(t, v, i)
  assert(type(t) == "table", "add: first argument must be a table")
  if v == nil then return nil end
  if i ~= nil then
    table.insert(t, i, v)
  else
    t[#t + 1] = v
  end
  return v
end

function foreach(t, f)
  assert(type(t) == "table", "foreach: first argument must be a table")
  assert(type(f) == "function", "foreach: second argument must be a function")

  for i = 1, #t do
    f(t[i])
  end
end

-- Math functions

function flr(x)
    if x >= 0 then
        return x - (x % 1)
    else
        return x - (1 + (x % 1)) % 1
    end
end