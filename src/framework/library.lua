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