-- Keys
_keys = {
    Back     = 8,  -- BACKSPACE key
    Tab      = 9,  -- TAB key
    Enter    = 13, -- ENTER key
    CapsLock = 20, -- CAPS LOCK key
    Escape   = 27, -- ESC key
    Space    = 32, -- SPACEBAR key
    PageUp   = 33, -- PAGE UP key
    PageDown = 34, -- PAGE DOWN key
    End      = 35, -- END key
    Home     = 36, -- HOME key
    Left     = 37, -- LEFT ARROW key
    Up       = 38, -- UP ARROW key
    Right    = 39, -- RIGHT ARROW key
    Down     = 40,  -- DOWN ARROW key
    A = 65, -- A key
    B = 66, -- B key
    C = 67, -- C key
    D = 68, -- D key
    E = 69, -- E key
    F = 70, -- F key
    G = 71, -- G key
    H = 72, -- H key
    I = 73, -- I key
    J = 74, -- J key
    K = 75, -- K key
    L = 76, -- L key
    M = 77, -- M key
    N = 78, -- N key
    O = 79, -- O key
    P = 80, -- P key
    Q = 81, -- Q key
    R = 82, -- R key
    S = 83, -- S key
    T = 84, -- T key
    U = 85, -- U key
    V = 86, -- V key
    W = 87, -- W key
    X = 88, -- X key
    Y = 89, -- Y key
    Z = 90 -- Z key
}

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
    return math.floor(x)
end

function clamp(low, b, high)
    return math.min(math.max(b, low), high)
end